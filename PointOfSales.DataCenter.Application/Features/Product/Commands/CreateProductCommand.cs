﻿using AutoMapper;
using MediatR;
using PointOfSales.DataCenter.Application.DTOs;
using PointOfSales.DataCenter.Application.Interfaces.Repositories;
using PointOfSales.DataCenter.Application.Mappings;
using PointOfSales.Domain.Entities.Products;
using System.Threading;
using System.Threading.Tasks;

namespace PointOfSales.DataCenter.Application.Features.ProductFeatures.Commands
{
    public class CreateProductCommand : IRequest<Result<int>>, IMapFrom<Domain.Entities.Products.Product>
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public int ProductGroupId { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Products.Product, CreateProductCommand>();
            profile.CreateMap<CreateProductCommand, Domain.Entities.Products.Product>();
        }
        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<int>>
        {
            private readonly IMapper _mapper;
            private readonly IProductRepositoryAsync _productRepository;
            public CreateProductCommandHandler(IMapper mapper, IProductRepositoryAsync productRepository)
            {
                _mapper = mapper;
                _productRepository = productRepository;
            }
            public async Task<Result<int>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                var product = _mapper.Map<Domain.Entities.Products.Product>(command);
                if(await _productRepository.DoesBarCodeExist(product.Barcode))
                {
                    return Result<int>.Failure($"Product with Barcode {product.Barcode} already exists.");
                }
                var result = await _productRepository.AddAsync(product);
                return Result<int>.Success(result.Id);
            }
        }
    }
}
